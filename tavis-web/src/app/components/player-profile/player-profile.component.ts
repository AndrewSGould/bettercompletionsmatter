import { Component, OnInit } from '@angular/core';
import { TavisService } from 'src/app/services/tavis.service';
import { Game } from 'src/models/game';
import { Player } from 'src/models/player';

@Component({
  selector: 'app-player-profile',
  templateUrl: './player-profile.component.html',
  styleUrls: ['./player-profile.component.scss']
})
export class PlayerProfileComponent implements OnInit {

  constructor(tavisService: TavisService) {
    this.tavisService = tavisService;
  }

  tavisService: TavisService | null = null;
  players: Player[] = [];
  completedGames: Game[] = [];

  ngOnInit(): void {
  }

  retrieveCompletedGames() {
    this.tavisService?.retrieveCompletedGames().subscribe(data => {
      alert('finished!')
      console.log(data);
    });
  }

  verifyRandomGameEligibility() {
    this.tavisService?.verifyRandomGameEligibility().subscribe(data => {
      console.log(data);
    })
  }

  getFullPlayerCompatability() {
    this.tavisService?.getFullPlayerCompatability().subscribe(data => {
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

  hhUpdate() {
    this.tavisService?.hhUpdate().subscribe(data => {
      console.log(data);
    })
  }
}
