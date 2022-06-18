import { TestBed } from '@angular/core/testing';

import { TavisService } from './tavis.service';

describe('TavisService', () => {
  let service: TavisService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TavisService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
