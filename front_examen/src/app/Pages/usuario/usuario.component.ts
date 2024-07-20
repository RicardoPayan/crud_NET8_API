import { Component, inject, Input, OnInit } from '@angular/core';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import { FormBuilder, FormGroup,ReactiveFormsModule} from '@angular/forms';
import { UsuarioService } from '../../Services/usuario.service';
import { Router } from '@angular/router';
import { Usuario } from '../../Models/Usuario';

@Component({
  selector: 'app-usuario',
  standalone: true,
  imports: [MatFormFieldModule,MatInputModule,MatButtonModule,ReactiveFormsModule],
  templateUrl: './usuario.component.html',
  styleUrl: './usuario.component.css'
})
export class UsuarioComponent implements OnInit {
  @Input('id')idUsuario!:number
  private usuarioServicio = inject(UsuarioService)
  public formBuild = inject(FormBuilder);

  public formUsuario:FormGroup=this.formBuild.group({
    nombre:[''],
    direccion:[''],
    telefono:[''],
    codigoPostal:[''],
    tipo:[0],
    estado:[''],
    ciudad:[''],
    login:[''],
    password:['']
  })

  constructor(private router:Router){}

   ngOnInit():void{
    if(this.idUsuario !=0 ){
      this.usuarioServicio.obtener(this.idUsuario).subscribe({
        next:(data)=>{
          this.formUsuario.patchValue({
            nombre:data.nombre,
            direccion:data.direccion,
            telefono:data.telefono,
            codigoPostal:data.codigoPostal,
            tipo:data.tipo,
            estado:data.estado,
            ciudad:data.ciudad,
            login:data.login,
            
          })
        },
        error:(err)=>{
          console.log(err)
        }
      })
    }
  }

guardar(){
  const objeto:Usuario ={
    idUsuario : this.idUsuario,
    nombre : this.formUsuario.value.nombre,
    direccion : this.formUsuario.value.direccion,
    telefono : this.formUsuario.value.telefono,
    codigoPostal : this.formUsuario.value.codigoPostal,
    tipo : +this.formUsuario.value.tipo,
    estado : this.formUsuario.value.estado,
    ciudad : this.formUsuario.value.ciudad,
    login : this.formUsuario.value.login,
    password : this.formUsuario.value.password,
  }

  if(this.idUsuario == 0){
    this.usuarioServicio.crear(objeto).subscribe({
      next:(data)=>{
        if(data.isSuccess){
          this.router.navigate(["/inicio"]);
        }else{
          alert('No se pudo guardar')
        }
      },
      error:(error)=>{
        console.log(error)
        if(error.status === 400){
          console.error('Detalles del error:', error.error.errors);
        }
      }
    })
  }else{
    this.usuarioServicio.editar(objeto).subscribe({
      next:(data)=>{
        if(data.isSuccess){
          this.router.navigate(['/inicio']);
        }else{
          alert('No se pudo editar')
        }
      },
      error:(err)=>{
        console.log(err)
      }
    })
  }
  
}

volver(){
  this.router.navigate(["/inicio"])
}


}
