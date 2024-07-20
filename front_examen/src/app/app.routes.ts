import { Routes } from '@angular/router';
import { InicioComponent } from './Pages/inicio/inicio.component';
import { LoginComponent } from './Pages/login/login.component';
import { UsuarioComponent } from './Pages/usuario/usuario.component';

export const routes: Routes = [
    {path:'', component:LoginComponent},
    {path:'inicio', component:InicioComponent},
    {path:'usuario/:id', component:UsuarioComponent},
];
