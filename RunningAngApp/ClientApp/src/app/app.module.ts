import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ListRunnersComponent } from './list-runners/list-runners.component';
import { RunnerDetailsComponent } from './runner-details/runner-details.component';
import { RunersService } from './services/ruuners.service';
import { PagerService } from './services/pager.service';
import { CalcComponent } from './calc/calc.component';
import { PageListRunnersComponent } from './page-list-runners/page-list-runners.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ListRunnersComponent,
    RunnerDetailsComponent,
    CalcComponent,
    PageListRunnersComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'list-runner', component: PageListRunnersComponent },
      { path: "details/:id", component: RunnerDetailsComponent },
      { path: "calc/:id", component: CalcComponent },
    ])
  ],
  providers: [
    RunersService, PagerService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
