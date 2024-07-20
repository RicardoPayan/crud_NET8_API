import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule} from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '../../Services/usuario.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private usuarioServicio = inject(UsuarioService)
  public formBuild = inject(FormBuilder);

 
  public formLogin:FormGroup=this.formBuild.group({
    login: ['', Validators.required],
    password: ['', Validators.required]
  })

  constructor(private router:Router){}

  onSubmit(): void {
    if (this.formLogin.valid) {
      const { login, password } = this.formLogin.value;
      this.usuarioServicio.login(login, password).subscribe({
        next: () => {
          this.router.navigate(['/inicio']);
        },
        error: (error) => {
          console.error('Error al intentar hacer login:', error);
          
          alert('No se encontro usuario o contraseña incorrecta')
        }
      });
    }
  }

 
}
