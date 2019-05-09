import { Component, OnInit } from '@angular/core';
import { RunnerRaceDetailDTO } from '../models/RunnerDTO';
import { RunersService } from '../services/ruuners.service';
import { ActivatedRoute } from "@angular/router";
import { RaceCalculator } from '../models/RunCalculator';

@Component({
  selector: 'app-calc',
  templateUrl: './calc.component.html',
  styleUrls: ['./calc.component.css']
})
export class CalcComponent implements OnInit {

  public ListOfRaces;
  public LastRace;
  public RaceTime: string;
  private ukan;
  public runner: RunnerRaceDetailDTO;

  constructor(private runnerService: RunersService, private route: ActivatedRoute) { }

  getRaces() {
    this.runnerService.getRaceList()
    .subscribe(races => this.ListOfRaces = races);
  }

  getLastRace(ukan) {
    this.runnerService.getLastRace(ukan).subscribe(LastRace => this.LastRace = LastRace);

  }

  calculateRaceDetails(selectedRace: any) {

    if (selectedRace == "0") {

      //this.forecasts = this.cacheForecasts;
      console.log(selectedRace);
    }
    else {
      console.log(selectedRace);
      var lastRun = this.LastRace;
      var calc = new RaceCalculator();
      var result = calc.CalculatePredicion(selectedRace, this.LastRace.Distance, this.LastRace.RaceTime);
      var time = calc.FormatTime(result);
      console.log(time);
      this.RaceTime = time;
      

      //api/RaceEventService?ukan=3273942
    }
  }
   

  ngOnInit() {

    const ukan = +this.route.snapshot.params["id"];

    this.getRaces();
    this.getLastRace(ukan);

  }

}
