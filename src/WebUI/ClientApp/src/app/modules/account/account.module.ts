import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { LoginComponent } from './pages/login/login.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { RegisterComponent } from './pages/register/register.component';
import { RequestResetComponent } from './pages/requestreset/request-reset.component';


const routes: Routes = [
  { path: '', redirectTo: 'overview' },
  {
    path: 'login', component: LoginComponent
  },
  {
    path: 'logout', component: LogoutComponent
  },
  {
    path: 'requestreset', component: RequestResetComponent,
  },
  {
    path: 'register', component: RegisterComponent,
  }
]

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    RequestResetComponent,
    LogoutComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    SharedModule,
  ],
  providers: [

  ]
})
export class AccountModule { }
