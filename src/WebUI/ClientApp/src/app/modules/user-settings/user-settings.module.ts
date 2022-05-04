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
import { ReactiveFormsModule } from '@angular/forms';
import { ServiceConnectionsPage } from './pages/service-connections/service-connections.page';
import { UserSettingsServiceConnectionsResolver } from './resolvers/user-settings-service-connections.resolver';
import { ServiceConnectionSnippetComponent } from './components/service-connection-snippet/service-connection-snippet.component';


const routes: Routes = [
  { path: '', redirectTo: 'profile' },
  {
    path: 'profile', component: UserProfileSettingsComponent,
    resolve: { profileInfoResponse: UserSettingsProfileInfoResolver }
  },
  {
    path: 'connections', component: ServiceConnectionsPage,
    resolve: { serviceConnectionsResponse: UserSettingsServiceConnectionsResolver }
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
    ServiceConnectionsPage,
    ServiceConnectionSnippetComponent,
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ],
  providers: [
    UserSettingsProfileInfoResolver,
    UserSettingsAccountInfoResolver,
    UserSettingsServiceConnectionsResolver
  ]
})
export class UserSettingsModule { }
