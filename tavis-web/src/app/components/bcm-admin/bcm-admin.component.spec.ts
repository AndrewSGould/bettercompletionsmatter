import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BcmAdminComponent } from './bcm-admin.component';

describe('BcmAdminComponent', () => {
  let component: BcmAdminComponent;
  let fixture: ComponentFixture<BcmAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BcmAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BcmAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
