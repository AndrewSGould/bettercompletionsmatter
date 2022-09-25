import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncManagerComponent } from './sync-manager.component';

describe('SyncManagerComponent', () => {
  let component: SyncManagerComponent;
  let fixture: ComponentFixture<SyncManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SyncManagerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SyncManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
