import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { SubscriptionViewConfigComponent } from './pages/config/subscription-view-config.component';
import { SubscriptionViewNavMenuComponent } from './components/nav-menu/subscription-view-nav-menu.component';
import { SubscriptionViewComponent } from './subscription-view.component';
import { SubscriptionViewOverviewComponent } from './pages/overview/subscription-view-overview.component';
import { SubscriptionViewPostsComponent } from './modules/posts/subscription-view-posts.component';

const routes: Routes = [
  { path: '', redirectTo: 'overview' },
  { path: 'config', component: SubscriptionViewConfigComponent },
  { path: 'overview', component: SubscriptionViewOverviewComponent },
  {
    path: 'posts', component: SubscriptionViewPostsComponent,
    loadChildren: () => import("./modules/posts/subscription-view-posts.module").then(m => m.SubscriptionViewPostsModule)
  },
]

@NgModule({
  declarations: [
    SubscriptionViewConfigComponent,
    SubscriptionViewNavMenuComponent,
    SubscriptionViewComponent,
    SubscriptionViewOverviewComponent,
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ]
})
export class SubscriptionViewModule { }
