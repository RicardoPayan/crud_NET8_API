import { ChangeDetectionStrategy,Component, inject } from '@angular/core';
import {MatCardModule} from '@angular/material/card'
import {MatTableModule} from '@angular/material/table'
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { UsuarioService } from '../../Services/usuario.service';
import { Usuario } from '../../Models/Usuario';
import { Router } from '@angular/router';


//Componenten de la pagina de inicio donde estara el listado de todos los registros


@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [MatButtonModule,MatCardModule,MatTableModule,MatIconModule],
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.css'
})
export class InicioComponent {

  private usuarioServicio = inject(UsuarioService)
  public listaUsuarios:Usuario[] = []
  public displayedColumns : string[] = ["nombre","direccion","telefono","codigoPostal", "tipo","estado","ciudad","accion"]

  //Funcion para obtener todos los usuarios
  obtenerUsuarios(){
    this.usuarioServicio.lista().subscribe({
      next:(data)=>{
        if(data.length > 0 ){
          this.listaUsuarios = data
        }
      },
      error:(err)=>{
        console.log(err.message)
      }
    })
  }

  //Creando el enrutado que llevara a las diferentes acciones del CRUD
  constructor(private router:Router){
    this.obtenerUsuarios()
  }

    nuevo(){
      this.router.navigate(['/usuario',0])
    }

    editar(objeto:Usuario){
      this.router.navigate(['/usuario', objeto.idUsuario])
    }

    eliminar(objeto:Usuario){
      if(confirm(`Desea eliminar al usuario: ${objeto.nombre}`)){
        this.usuarioServicio.eliminar(objeto.idUsuario).subscribe({
          next:(data)=>{
            if(data.isSuccess){
              this.obtenerUsuarios();
            }else{
              alert("No se pudo eliminar")
            }
          },
          error:(err)=>{
            console.log(err.message)
          }
        })
      }
    }
  
    volver(){
      this.router.navigate(["/"])
    }


}
