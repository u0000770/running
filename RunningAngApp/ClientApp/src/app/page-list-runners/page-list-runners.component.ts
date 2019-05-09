import { Component, OnInit } from '@angular/core';
import { PagerService } from '../services/pager.service';
import { RunersService } from '../services/ruuners.service';

@Component({
  selector: 'app-page-list-runners',
  templateUrl: './page-list-runners.component.html',
  styleUrls: ['./page-list-runners.component.css']
})
export class PageListRunnersComponent implements OnInit {

  constructor(private runnerService: RunersService, private pagerService: PagerService) { }

  // array of all items to be paged
 // private allItems: any[];

  // pager object
  pager: any = {};

  // paged items
  pagedItems: any[];

  public runners;


  getRunners(): void {
    this.runnerService.getRunners()
      .subscribe(runners => { this.runners = runners; this.setPage(1) });
  }


  ngOnInit() {
    this.getRunners();
  }



  setPage(page: number) {
    if (page < 1 || page > this.pager.totalPages) {
      return;
    }

    // get pager object from service
    this.pager = this.pagerService.getPager(this.runners.length, page);

    // get current page of items
    this.pagedItems = this.runners.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

}
