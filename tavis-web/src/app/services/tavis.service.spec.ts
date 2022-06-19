import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TavisService } from './tavis.service';

describe('TavisService', () => {
  let service: TavisService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(TavisService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
