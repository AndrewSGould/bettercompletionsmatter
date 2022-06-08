import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BcmService } from 'src/app/services/bcm.service';
import { Game } from 'src/models/game';

@Component({
  selector: 'app-completed-games',
  templateUrl: './completed-games.component.html',
  styleUrls: ['./completed-games.component.scss']
})
export class CompletedGamesComponent implements OnInit {
  completedGameForm = new FormGroup({
    title: new FormControl(''),
    ratio: new FormControl(''),
    time: new FormControl(''),
    value: new FormControl('')
  });

  constructor(bcmService: BcmService) { 
    this.bcmService = bcmService;
  }

  bcmService: BcmService | null = null;

  ngOnInit(): void {
  }

  onSubmit() {
    let gameValue = this.completedGameForm.controls['ratio'].value * 
      Math.sqrt(this.completedGameForm.controls['ratio'].value) * 
      this.completedGameForm.controls['time'].value;
    
    this.completedGameForm.controls['value'].setValue(Math.ceil(gameValue));

    var gameForm = this.completedGameForm.value;

    var game = new Game();
    game = gameForm;

    this.bcmService?.addCompletedGame(game).subscribe();
  }
}
