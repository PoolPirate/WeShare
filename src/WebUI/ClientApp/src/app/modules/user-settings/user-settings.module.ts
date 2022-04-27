import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../../shared/shared.module';
import { UserSettingsNavMenuComponent } from './components/nav-menu/user-settings-nav-menu.component';
import { UserAccountSettingsComponent } from './pages/account/user-account-settings.component';
import { UserProfileSettingsComponent } from './pages/profile/user-profile-settings.component';
import { UserSettingsAccountInfoResolver } from './resolvers/user-settings-accountinfo.resolver';
import { UserSettingsProfileInfoResolver } from './resolvers/user-settings-profileinfo.resolver';
import { UserSettingsComponent } from './user-settings.component';


const routes: Routes = [
  { path: '', redirectTo: 'profile' },
  {
    path: 'profile', component: UserProfileSettingsComponent,
    resolve: { profileInfoResponse: UserSettingsProfileInfoResolver }
  },
  {
    path: 'personal', component: UserAccountSettingsComponent,
    resolve: { accountInfoResponse: UserSettingsAccountInfoResolver }
  },
]

@NgModule({
  declarations: [
    UserSettingsComponent,
    UserSettingsNavMenuComponent,
    UserAccountSettingsComponent,
    UserProfileSettingsComponent,
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes)
  ],
  providers: [
    UserSettingsProfileInfoResolver,
    UserSettingsAccountInfoResolver
  ]
})
export class UserSettingsModule { }
