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
    this.tavisService?.getAllPlayers().subscribe(data => {
      this.players = data;
    });
  }

  updatePlayerProfile(playerId: any) {
    this.tavisService?.getPlayersGames(playerId).subscribe(data => {
      this.completedGames = data;
    })
  }

  retrieveCompletedGames(playerId: any) {
    this.tavisService?.retrieveCompletedGames(playerId).subscribe(data => {
      alert('finished!');
      console.log(data);
    });
  }

  generateRandomGame() {
    this.tavisService?.generateBcmRandomGame().subscribe(data => {
      console.log(data);
    })
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

  raidBossSync() {
    this.tavisService?.raidBossSync().subscribe(data => {
      console.log(data);
    }); 
  }

  calculateDamage() {
    this.tavisService?.calculateDamage().subscribe(data => {
      console.log(data);
    }); 
  }
}
