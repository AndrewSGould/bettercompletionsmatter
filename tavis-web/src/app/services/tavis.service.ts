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

  retrieveCompletedGames(): Observable<any> {
    return this.http.get(baseUrl + `bcm/ta_sync`);
  }

  verifyRandomGameEligibility(): Observable<any> {
    return this.http.get(baseUrl + `bcm/verifyRandomGameEligibility`);
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

  hhUpdate(): Observable<any> {
    return this.http.get(baseUrl + `raidboss/hh`);
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
