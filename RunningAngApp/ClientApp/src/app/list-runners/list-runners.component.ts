import { Component, OnInit, Inject } from '@angular/core';
import { RunersService } from '../services/ruuners.service';

@Component({
  selector: 'app-list-runners',
  templateUrl: './list-runners.component.html'
})
export class ListRunnersComponent implements OnInit{
  public runners;

  constructor(private runnerService: RunersService) { }

  getRunners(): void {
    this.runnerService.getRunners()
      .subscribe(runners => this.runners = runners);
  }


  ngOnInit() {
    this.getRunners();
  }


}


