import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RunnerDetailsComponent } from './runner-details.component';

describe('RunnerDetailsComponent', () => {
  let component: RunnerDetailsComponent;
  let fixture: ComponentFixture<RunnerDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RunnerDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RunnerDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
