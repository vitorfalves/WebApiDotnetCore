import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  baseURL = 'http://localhost:5000/Eventos';

constructor(private http: HttpClient) { }

    getEventoList(): Observable<Evento[]>{
      return this.http.get<Evento[]>(`${this.baseURL}/GetEventosList`);
    }

    getEventoByTema(tema: string): Observable<Evento[]>{
      return this.http.get<Evento[]>(`${this.baseURL}/GetEventosByTema/${tema}`);
    }

    getEventoById(id: number): Observable<Evento>{
      return this.http.get<Evento>(`${this.baseURL}/GetEventosById/${id}`);
    }

}
