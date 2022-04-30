import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { PostViewComponent } from './post-view.component';
import { PostViewNavMenuComponent } from './components/nav-menu/post-view-nav-menu.component';
import { PostViewHeadersPage } from './pages/headers/post-view-headers.page';
import { PostViewPayloadPage } from './pages/payload/post-view-payload.page';
import { PostViewOverviewPage } from './pages/overview/post-view-overview.page';

const routes: Routes = [
  { path: '', redirectTo: 'overview' },
  { path: 'overview', component: PostViewOverviewPage },
  { path: 'headers', component: PostViewHeadersPage },
  { path: 'payload', component: PostViewPayloadPage }
]

@NgModule({
  declarations: [
    PostViewNavMenuComponent,
    PostViewComponent,

    PostViewOverviewPage,
    PostViewHeadersPage,
    PostViewPayloadPage
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ]
})
export class PostViewModule { }
