import { Component, OnInit } from '@angular/core';
import { RunersService } from '../services/ruuners.service';
import { ActivatedRoute } from "@angular/router";
import { RunnerRaceDetailDTO, EventRaceTimesDTO } from '../models/RunnerDTO';

@Component({
  selector: 'app-runner-details',
  templateUrl: './runner-details.component.html',
  styleUrls: ['./runner-details.component.css']
})
export class RunnerDetailsComponent implements OnInit {

  public runner: RunnerRaceDetailDTO;
  constructor(private runnersService: RunersService, private route: ActivatedRoute) { }

   
  ngOnInit(): void {
    const ukan = +this.route.snapshot.params["id"];

    this.runnersService.getRunnerDetail(ukan).subscribe(runner => this.runner = runner);

  }
}
