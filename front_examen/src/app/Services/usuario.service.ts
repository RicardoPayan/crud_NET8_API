import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResponseAPI } from '../Models/ResponseAPI';
import { Usuario } from '../Models/Usuario';
import { appsettings } from '../Settings/appsettings';


@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private http = inject(HttpClient)
  private apiUrl = appsettings.apiUrl + "/Usuario"
  constructor() { }

  lista(){
    return this.http.get<Usuario[]>(this.apiUrl)
  }

  obtener(id:number){
    return this.http.get<Usuario>(`${this.apiUrl}/${id}`)
  }

  crear(objeto:Usuario){
    return this.http.post<ResponseAPI>(this.apiUrl, objeto)
  }

  editar(objeto:Usuario){
    return this.http.put<ResponseAPI>(this.apiUrl, objeto)
  }

  eliminar(id:number){
    return this.http.delete<ResponseAPI>(`${this.apiUrl}/${id}`)
  }


  login(login: string, password: string): Observable<ResponseAPI> {
    return this.http.post<ResponseAPI>(`${this.apiUrl}/login`, { login, password })
  }

}

