export interface RunnerDTO {
  runnerName: string;
  runnerUKAN: number;
}


export interface RunnerRaceDetailDTO {
  ukaNumber: string ;
  Name: string;
  listOfRaces: Array<EventRaceTimesDTO>;

}


export interface RaceListDTO {

  Title: string;
  Distance: number;
}


export interface RunnerLastRaceDTO {
        RaceTime: number;
        Distance: number;
}


export interface EventRaceTimesDTO {
  RaceId: number;
       
  RaceDistance: string;
       
  RaceTitle: string;
       
  RaceTargetTime: string;
      
  RaceActualTime: string;

  RaceDate: string;
     
  TargetTime: number;

       
}

