export type Animal = {
    Id: number,
    Name: string,
    Age: number,
    Specie: string
}
export type Zoo = {
    Id: number,
    Name: string,
    Animals: Animal[]
}