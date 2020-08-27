import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
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

    postEvento(evento: Evento){
      return this.http.post(this.baseURL, evento);//`${this.baseURL}`
    }

    putEvento(evento: Evento){
      return this.http.put(`${this.baseURL}/${evento.id}`, evento );
    }

    deleteEvento(id: number){
      return this.http.delete(`${this.baseURL}/${id}`);
    }

    postUpload(file: File, fileName: string){
      const fileToUpload = <File>file[0];
      const formData = new FormData();
      formData.append('file', fileToUpload, fileName);

      return this.http.post(`${this.baseURL}/upload`, formData);
    }
  
}
