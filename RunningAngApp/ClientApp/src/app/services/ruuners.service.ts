import { Injectable } from '@angular/core';
import { RunnerDTO, RunnerRaceDetailDTO, RaceListDTO, RunnerLastRaceDTO } from '../models/RunnerDTO';
import { Observable } from 'rxjs';
import { HttpClient} from '@angular/common/http';

@Injectable()
export class RunersService {

  public runners: RunnerDTO[];
  public runner: RunnerRaceDetailDTO;
  public races: RaceListDTO[];
  public lastRace: RunnerLastRaceDTO;

  constructor(private http: HttpClient) { }

  private runnerUrl = 'https://rundistance.azurewebsites.net/api/';



  // api/RaceEventService?ukan=3273942
  getLastRace(ukan): Observable<RunnerLastRaceDTO> {
    return this.http.get<RunnerLastRaceDTO>(this.runnerUrl + "RaceEventService?" + "ukan=" + ukan)
}


   getRunners(): Observable<RunnerDTO[]> {
     return this.http.get<RunnerDTO[]>(this.runnerUrl + "RunnerService")
  }


  // api/RaceEventService

  getRunnerDetail(ukan): Observable<RunnerRaceDetailDTO> {
    return this.http.get<RunnerRaceDetailDTO>(this.runnerUrl + "RunnerService"+ "?ukan=" + ukan)
  }


    // api/RaceEventService
  getRaceList(): Observable<RaceListDTO[]> {
    var myurl = this.runnerUrl + "RaceEventService/";
    var fish = this.http.get<RaceListDTO[]>(myurl);
    
    console.log(fish);
    return fish;
  }

}
