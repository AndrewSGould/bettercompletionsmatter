import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TavisService } from 'src/app/services/tavis.service';
import { Game } from 'src/models/game';
import { Player } from 'src/models/player';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(tavisService: TavisService) {
    this.tavisService = tavisService;
  }

  tavisService: TavisService | null = null;
  players: Player[] = [];
  completedGames: Game[] = [];

  ngOnInit(): void {
  }
}
