import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from 'src/models/game';
import { Player } from 'src/models/player';

const baseUrl = 'http://localhost:4200/api/';

@Injectable({
  providedIn: 'root'
})
export class TavisService {

  constructor(private http: HttpClient) { }
  
  getAllPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(baseUrl + 'players/getAll');
  }

  getPlayersGames(playerId: any): Observable<Game[]> {
    return this.http.get<Game[]>(baseUrl + `players/getPlayersGames?playerId=${playerId}`);
  }

  retrieveCompletedGames(playerId: any): Observable<any> {
    return this.http.get(baseUrl + `datasync/syncAllData`);
  }

  generateTavisRandomGame(playerId: any): Observable<any> {
    return this.http.get(baseUrl + `Tavis/getRandomGame?playerId=${playerId}`);
  }

  verifyRandomGameEligibility(): Observable<any> {
    return this.http.get(baseUrl + `Tavis/verifyRandomGameEligibility`);
  }

  getFullPlayerCompatability(): Observable<any> {
    return this.http.get(baseUrl + `players/getFullPlayerCompatability`);
  }

  updateGameInfo(): Observable<any> {
    return this.http.get(baseUrl + `datasync/testSyncGameInfo`);
  }

  testGwgParse(): Observable<any> {
    return this.http.get(baseUrl + `datasync/testGwgParse`);
  }

  raidBossSync(): Observable<any> {
    return this.http.get(baseUrl + `raidboss/ta_sync`)
  }

  calculateDamage(): Observable<any> {
    return this.http.get(baseUrl + `raidboss/calculateDamage`);
  }

  getAllGames(): Observable<Game[]> {
    return this.http.get<Game[]>(baseUrl + 'games/getAll');
  }

  addCompletedGame(data: Game): Observable<any> {
    return this.http.post(baseUrl + 'games/addCompletedGame', data);
  }

  create(data: any): Observable<any> {
    return this.http.post(baseUrl, data);
  }

  update(id: any, data: any): Observable<any> {
    return this.http.put(`${baseUrl}/${id}`, data);
  }

  delete(id: any): Observable<any> {
    return this.http.delete(`${baseUrl}/${id}`);
  }
}
