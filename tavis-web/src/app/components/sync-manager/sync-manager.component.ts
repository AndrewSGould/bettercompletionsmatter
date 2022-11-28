import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignalrService } from 'src/app/services/signalr.service';
import { TavisService } from 'src/app/services/tavis.service';

@Component({
  selector: 'app-sync-manager',
  templateUrl: './sync-manager.component.html',
  styleUrls: ['./sync-manager.component.scss']
})
export class SyncManagerComponent implements OnInit {

  constructor(tavisService: TavisService, public signalRService: SignalrService) {
    this.tavisService = tavisService;
  }

  private tavisService: TavisService | null = null;
  playersToSync: number = 0;
  totalEstimatedTime: number | null = null;
  totalTaHits: number | null = null;
  scannedPlayerCount: number = -1;
  syncStatus: string | null = null;
  progressBarStatus: number = 0;

  ngOnInit(): void {
    this.tavisService?.syncInfo().subscribe(data => {
      this.totalEstimatedTime = data.estimatedRuntime;
      this.totalTaHits = data.estimatedTaHits;
      this.playersToSync = data.playerCount;
    });

    this.signalRService.startConnection();
    this.signalRService.addSyncDataListener();  
    
    this.signalRService.syncSignal.subscribe(data => {
      this.syncStatus = data;
      this.scannedPlayerCount++;

      this.progressBarStatus = (this.scannedPlayerCount / this.playersToSync) * 100;
    })
  }

  fullSync() {
    this.tavisService?.fullSync().subscribe(data => {
      alert('finished!')
      console.log(data);
    });
  }

  syncLastMonthsCompletions() {
    this.tavisService?.syncLastMonthsCompletions().subscribe(data => {
      alert('finished!')
      console.log(data);
    });
  }

  updateGameInfo() {
    this.tavisService?.updateGameInfo().subscribe(data => {
      console.log(data);
    });
  }

  testGwgParse() {
    this.tavisService?.testGwgParse().subscribe(data => {
      console.log(data);
    });
  }
}
