import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from 'src/models/game';
import { Player } from 'src/models/player';

const baseUrl = 'http://localhost:4200/api/';

@Injectable({
  providedIn: 'root'
})
export class BcmService {

  constructor(private http: HttpClient) { }
  
  getAllPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(baseUrl + 'players/getAll');
  }

  getPlayersGames(playerId: any): Observable<Game[]> {
    return this.http.get<Game[]>(baseUrl + `players/getCompletedGames?playerId=${playerId}`);
  }

  retrieveCompletedGames(playerId: any): Observable<any> {
    return this.http.get(baseUrl + `players/retrieveCompletedGames?playerId=${playerId}`);
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
