import * as React from 'react'
import { RouteComponentProps } from "react-router";
import { Zoo, Animal } from '../types';

export let ZooFC : React.FunctionComponent<Zoo> = (m) => {
  return <div>
    <div>Name: {m.Name}</div>
  </div>
}

export let AnimalFC : React.FunctionComponent<Animal> = (m) => {
  return <div style={{border:"dotted"}}>
    <div>Name: {m.Name}</div>
    <div>Age: {m.Age}</div>
    <div>Specie: {m.Specie}</div>
  </div>
}