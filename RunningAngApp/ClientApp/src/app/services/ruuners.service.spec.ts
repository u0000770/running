import { TestBed, inject } from '@angular/core/testing';

import { RuunersService } from './ruuners.service';

describe('RuunersService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RuunersService]
    });
  });

  it('should be created', inject([RuunersService], (service: RuunersService) => {
    expect(service).toBeTruthy();
  }));
});
