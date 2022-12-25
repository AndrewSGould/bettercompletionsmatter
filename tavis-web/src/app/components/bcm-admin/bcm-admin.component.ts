import { Component, Inject, OnInit } from '@angular/core';
import { TavisService } from 'src/app/services/tavis.service';
import { MatDialog } from '@angular/material/dialog';
import { RandomGamesDialog } from 'src/app/dialogs/random-games/random-games-dialog';

@Component({
  selector: 'app-bcm-admin',
  templateUrl: './bcm-admin.component.html',
  styleUrls: ['./bcm-admin.component.scss']
})
export class BcmAdminComponent implements OnInit {

  constructor(private tavisService: TavisService, public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  verifyRandomGameEligibility() {
    this.tavisService?.verifyRandomGameEligibility().subscribe(data => {
      this.dialog.open(RandomGamesDialog, {
        data: data,
        height: '80%',
        width: '50%',
      });
    })
  }

  produceBcmReport() {
    this.tavisService?.produceBcmReport().subscribe(data => {
      console.log(data);
    })
  }
}

