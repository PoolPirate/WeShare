import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { OAuth2CallbackPage } from './pages/oauth2-callback/oauth2-callback.page';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { VerifyEmailComponent } from './pages/verify-email/verify-email.component';

const routes: Routes = [
  { path: 'verifyemail/:callbackSecret', component: VerifyEmailComponent },
  { path: 'resetpassword/:callbackSecret', component: ResetPasswordComponent },
  { path: 'discord-callback', component: OAuth2CallbackPage },
]

@NgModule({
  declarations: [
    ResetPasswordComponent,
    VerifyEmailComponent,
    OAuth2CallbackPage,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ]
})
export class CallbackModule { }
