import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ShareViewComponent } from './share-view.component';
import { ShareViewNavMenuComponent } from './nav-menu/share-view-nav-menu.component';
import { ShareViewOverviewComponent } from './pages/overview/share-view-overview.component';
import { ShareViewPostsComponent } from './pages/posts/share-view-posts.component';
import { MaterialModule } from '../material/material.module';
import { ViewSharePostsResolver } from './resolvers/share-view-posts.resolver';
import { ViewShareSecretsResolver } from './resolvers/share-view-secrets.resolver';
import { ShareViewSettingsComponent } from '../share-view-settings/share-view-settings.component';
import { ShareViewEditComponent } from '../share-view-edit/share-view-edit.component';

const routes: Routes = [
  { path: '', redirectTo: 'overview' },
  { path: 'overview', component: ShareViewOverviewComponent },
  {
    path: 'posts', component: ShareViewPostsComponent,
    resolve: { postsResponse: ViewSharePostsResolver }
  },
  {
    path: 'settings', component: ShareViewSettingsComponent,
    loadChildren: () => import("../share-view-settings/share-view-settings.module").then(m => m.ShareViewSettingsModule),
    resolve: { secretsResponse: ViewShareSecretsResolver }
  },
  {
    path: 'edit', component: ShareViewEditComponent,
    loadChildren: () => import("../share-view-edit/share-view-edit.module").then(m => m.ShareViewEditModule)
  }
]

@NgModule({
  declarations: [
    ShareViewComponent,
    ShareViewNavMenuComponent,
    ShareViewOverviewComponent,
    ShareViewPostsComponent
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ],
  providers: [
    ViewSharePostsResolver,
    ViewShareSecretsResolver,
  ]
})
export class ShareViewModule { }
