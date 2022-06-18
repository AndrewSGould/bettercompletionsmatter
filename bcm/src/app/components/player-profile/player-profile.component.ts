import { Component, OnInit } from '@angular/core';
import { BcmService } from 'src/app/services/bcm.service';
import { Game } from 'src/models/game';
import { Player } from 'src/models/player';

@Component({
  selector: 'app-player-profile',
  templateUrl: './player-profile.component.html',
  styleUrls: ['./player-profile.component.scss']
})
export class PlayerProfileComponent implements OnInit {

  constructor(bcmService: BcmService) {
    this.bcmService = bcmService;
  }

  bcmService: BcmService | null = null;
  players: Player[] = [];
  completedGames: Game[] = [];

  ngOnInit(): void {
    this.bcmService?.getAllPlayers().subscribe(data => {
      this.players = data;
    });
  }

  updatePlayerProfile(playerId: any) {
    this.bcmService?.getPlayersGames(playerId).subscribe(data => {
      this.completedGames = data;
    })
  }

  retrieveCompletedGames(playerId: any) {
    this.bcmService?.retrieveCompletedGames(playerId).subscribe(data => {
      alert('finished!');
      console.log(data);
    });
  }

  generateRandomGame(playerId: any) {
    this.bcmService?.generateBcmRandomGame(playerId).subscribe(data => {
      console.log(data);
    })
  }

  verifyRandomGameEligibility() {
    this.bcmService?.verifyRandomGameEligibility().subscribe(data => {
      console.log(data);
    })
  }

  getFullPlayerCompatability() {
    this.bcmService?.getFullPlayerCompatability().subscribe(data => {
      console.log(data);
    });
  }

  updateGameInfo() {
    this.bcmService?.updateGameInfo().subscribe(data => {
      console.log(data);
    });
  }

  testGwgParse() {
    this.bcmService?.testGwgParse().subscribe(data => {
      console.log(data);
    });
  }
}
