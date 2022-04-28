import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { MaterialModule } from '../material/material.module';
import { UnreceivedPostsComponent } from './components/unreceived-posts.component';
import { DashboardMainComponent } from './pages/main/dashboard-main.component';
import { DashboardSubscriptionSnippetsResolver } from './resolvers/dashboard-subscriptionsnippets.resolver';

const routes: Routes = [
  { path: '', redirectTo: 'main' },
  {
    path: 'main', component: DashboardMainComponent,
    resolve: { subscriptionSnippetsResponse: DashboardSubscriptionSnippetsResolver }
  }
]

@NgModule({
  declarations: [
    DashboardMainComponent,
    UnreceivedPostsComponent
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
  providers: [

  ]
})
export class DashboardModule { }
