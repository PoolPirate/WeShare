import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { WeShareClient } from '../../services/weshareclient';
import { ShareSnippetComponent } from './components/share-snippet/share-snippet.component';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { MatIconModule } from '@angular/material/icon';
import { LikeButtonComponent } from './components/like-button/like-button.component';
import { MatButtonModule } from '@angular/material/button';
import { NotFoundComponent } from './pages/notfound/notfound.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { SubscriptionSnippetComponent } from './components/subscription-snippet/subscription-snippet.component';
import { PostSnippetComponent } from './components/post-snippet/post-snippet.component';
import { MatCardModule } from '@angular/material/card';
import { MatExpansionModule } from '@angular/material/expansion';
import { CreateButtonComponent } from './components/create-button/create-button.component';
import { PagedListHeaderComponent } from './components/paged-list-header/paged-list-header.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HeaderPairComponent } from './components/header-pair/header-pair.component';
import { SharedLoadingDialog } from './dialogs/loading/loading.dialog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SharedConfirmDialog } from './dialogs/confirm/confirm.dialog';
import { DialogService } from '../../services/dialogservice';
import { AuthService } from '../../services/authservice';
import { MatMenuModule } from '@angular/material/menu';
import { PostSendFailureSnippetComponent } from './components/post-send-failure-snippet/post-send-failure-snippet.component';
import { MatChipsModule } from '@angular/material/chips';

@NgModule({
  declarations: [
    NotFoundComponent,
    ForbiddenComponent,
    ShareSnippetComponent,
    LikeButtonComponent,
    SubscriptionSnippetComponent,
    PostSnippetComponent,
    CreateButtonComponent,
    PagedListHeaderComponent,
    HeaderPairComponent,
    PostSendFailureSnippetComponent,

    SharedLoadingDialog,
    SharedConfirmDialog
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([]),

    LoadingBarRouterModule,

    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatExpansionModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatChipsModule,
  ],
  exports: [ 
    NotFoundComponent,
    ForbiddenComponent,

    LikeButtonComponent,
    CreateButtonComponent,

    SharedLoadingDialog,
    SharedConfirmDialog,

    ShareSnippetComponent,
    SubscriptionSnippetComponent,
    PostSnippetComponent,
    PostSendFailureSnippetComponent,

    HeaderPairComponent,

    PagedListHeaderComponent,

    CommonModule,

    LoadingBarRouterModule,

    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatExpansionModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatChipsModule,
  ],
  providers: [
    AuthService,
    WeShareClient,
    DialogService
  ]
})
export class SharedModule { }
