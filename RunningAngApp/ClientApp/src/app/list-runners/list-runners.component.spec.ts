import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListRunnersComponent } from './list-runners.component';

describe('ListRunnersComponent', () => {
  let component: ListRunnersComponent;
  let fixture: ComponentFixture<ListRunnersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListRunnersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListRunnersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
