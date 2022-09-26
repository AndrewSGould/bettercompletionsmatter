import { Component, OnInit } from '@angular/core';
import { TavisService } from 'src/app/services/tavis.service';

@Component({
  selector: 'app-bcm-admin',
  templateUrl: './bcm-admin.component.html',
  styleUrls: ['./bcm-admin.component.scss']
})
export class BcmAdminComponent implements OnInit {

  constructor(private tavisService: TavisService) { }

  ngOnInit(): void {
  }

  verifyRandomGameEligibility() {
    this.tavisService?.verifyRandomGameEligibility().subscribe(data => {
      console.log(data);
    })
  }

  hhUpdate() {
    this.tavisService?.hhUpdate().subscribe(data => {
      console.log(data);
    })
  }
}
