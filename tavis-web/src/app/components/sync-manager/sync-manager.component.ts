import { Component, OnInit } from '@angular/core';
import { TavisService } from 'src/app/services/tavis.service';

@Component({
  selector: 'app-sync-manager',
  templateUrl: './sync-manager.component.html',
  styleUrls: ['./sync-manager.component.scss']
})
export class SyncManagerComponent implements OnInit {

  constructor(tavisService: TavisService) {
    this.tavisService = tavisService;
  }

  private tavisService: TavisService | null = null;
  playersToSync: number | null = null;
  totalEstimatedTime: number | null = null;
  totalTaHits: number | null = null;

  ngOnInit(): void {
    this.tavisService?.syncInfo().subscribe(data => {
      this.totalEstimatedTime = data.estimatedRuntime;
      this.totalTaHits = data.estimatedTaHits;
      this.playersToSync = data.playerCount;
    });
  }

  fullSync() {
    this.tavisService?.fullSync().subscribe(data => {
      alert('finished!')
      console.log(data);
    });
  }
}
