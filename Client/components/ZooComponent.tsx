import * as React from 'react'
import { Zoo, Animal } from '../types';
import { Map } from 'immutable'
import { AnimalFC, ZooFC } from '../containers/ZooContainers';
import { Set } from "immutable"


export type ZooComponentState = {
  zoo: { kind: "loaded", value: Zoo[] } | { kind: "loading" } | { kind: "error-or-not-found" } | { kind: "none" }
}
export type ZooComponentProps = {}


export class ZooComponent extends React.Component<ZooComponentProps, ZooComponentState> {

  constructor(props: ZooComponentProps) {
    super(props)
    this.state = {
      zoo: { kind: "none" }

    }
  }

  async processApiZooResponce(res: Response) {
    try {
      if (!res.ok)
        this.setState(s => ({ ...s, zoo: { kind: "error-or-not-found" } }))

      let res1 = await res.json()

      let zoo: Zoo[] = []
      zoo = res1.map((item): Zoo => {
        return { ...item.Zoo, Animals: item.Animals }
      })
      this.setState(s => ({ ...s, zoo: { kind: "loaded", value: zoo } }))
    }
    catch {
      this.setState(s => ({ ...s, zoo: { kind: "error-or-not-found" } }))
    }
  }

  getZoo() {
    this.setState(s => ({ ...s, zoo: { kind: "loading" } }), () => {

      let headers = { 'content-type': 'application/json' }

      fetch(`/zoo/GetAllAnimals`,
        {
          method: 'get',
          headers
        })
        .then(async res => {
          this.processApiZooResponce(res)
        })
    })
  }

  
  getZooBySpecie(specie:string) {
    this.setState(s => ({ ...s, zoo: { kind: "loading" } }), () => {

    let headers = { 'content-type': 'application/json' }

    fetch(`/zoo/GetAllAnimals?specie=${specie}`,
      {
        method: 'get',
        headers
      })
      .then(async res => {
        this.processApiZooResponce(res)
      })
    })
  }

  componentWillMount() {
    this.getZoo()
  }

  public render() {
    if (this.state.zoo.kind != "loaded") {
      return <p>{this.state.zoo.kind}</p>
    }

    return <div className='container-fluid row'>
      <div className="row">
        <div className='col-sm-6'>
          <h2>Zoo</h2>

          <div>
            Filter by:
            {
              Set(this.state.zoo.value.map(z => z.Animals.map(a => a.Specie)).reduce(a => a))
              .map(specie => <button style={{ margin: 5 }}
                  onClick={_ => this.getZooBySpecie(specie)}
              >{specie}</button>)
            }
            ...
            <button onClick={_ => this.getZoo()}>Clear Searc</button>
          </div>
          {this.state.zoo.value.map(p =>
            <div style={{ border: "solid" }}>
              {
                <div>Zoo: <ZooFC {...p} /></div>
              }
              <div>Zoo: ...</div>
              <div>
                Animals:
                <div style={{ paddingLeft: 10 }}>
                  {p.Animals.map(c => <AnimalFC {...c} />)}
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  }
}