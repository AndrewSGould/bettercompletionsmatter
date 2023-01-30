import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Game } from 'src/models/game';
import { Player } from 'src/models/player';
import { environment } from '../../environments/environment';

const baseUrl = environment.api.baseUrl;

@Injectable({
  providedIn: 'root'
})
export class TavisService {

  constructor(private http: HttpClient) { }

  fullSync(): Observable<any> {
    return this.http.get(baseUrl + `datasync/full`);
  }

  syncInfo(): Observable<any> {
    return this.http.get(baseUrl + `datasync/syncInfo`);
  }

  verifyRandomGameEligibility(): Observable<any> {
    return this.http.get(baseUrl + `bcm/verifyRandomGameEligibility`);
  }

  updateGameInfo(): Observable<any> {
    return this.http.get(baseUrl + `datasync/testSyncGameInfo`);
  }

  testGwgParse(): Observable<any> {
    return this.http.get(baseUrl + `datasync/testGwgParse`);
  }
  
  syncLastMonthsCompletions(): Observable<any> {
    return this.http.get(baseUrl + `datasync/lastmonthscompletions`);
  }

  produceBcmReport(): Observable<any> {
    return this.http.get(baseUrl + `bcm/produceBcmReport`);
  }

  produceStatReport(): Observable<any> {
    return this.http.get(baseUrl + `bcm/produceStatReport`);
  }

  produceCompletedGamesReport(): Observable<any> {
    return this.http.get(baseUrl + `bcm/produceCompletedGamesReport`);
  }

  unique14chars(): Observable<any> {
    return this.http.get(baseUrl + `bcm/unique14chars`);
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
