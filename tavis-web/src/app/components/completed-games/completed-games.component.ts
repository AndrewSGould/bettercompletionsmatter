import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TavisService } from 'src/app/services/tavis.service';
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

  constructor(tavisService: TavisService) { 
    this.tavisService = tavisService;
  }

  tavisService: TavisService | null = null;

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

    this.tavisService?.addCompletedGame(game).subscribe();
  }
}
