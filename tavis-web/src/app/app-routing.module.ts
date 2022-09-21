import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlayerProfileComponent } from './components/player-profile/player-profile.component';

const routes: Routes = [
  { path: '', redirectTo: 'player-profile', pathMatch: 'full' },
  { path: 'player-profile', component: PlayerProfileComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
