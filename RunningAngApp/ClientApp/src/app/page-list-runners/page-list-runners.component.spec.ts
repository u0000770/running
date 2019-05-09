import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageListRunnersComponent } from './page-list-runners.component';

describe('PageListRunnersComponent', () => {
  let component: PageListRunnersComponent;
  let fixture: ComponentFixture<PageListRunnersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageListRunnersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageListRunnersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
