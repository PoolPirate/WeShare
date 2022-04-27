import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WeShareClient } from '../../../../services/weshareclient';
import { ShareData, ShareInfo } from '../../../../types/share-types';
import { ShareService } from '../../../share-view/services/shareservice';

@Component({
  selector: 'share-view-edit-readme',
  templateUrl: './share-view-edit-readme.component.html'
})
export class ShareViewEditReadmeComponent {
  shareData: ShareData;

  readmeForm: FormGroup;

  ranForReadme: string = "";
  errorCode: number = 0;

  constructor(private weShareClient: WeShareClient,
    shareService: ShareService, formBuilder: FormBuilder) {
    this.shareData = shareService.shareData;

    this.readmeForm = formBuilder.group({
      readme: [this.shareData.shareInfo.readme, [Validators.maxLength(4096)]],
    });
  }

  updateReadme() {
    this.errorCode = 0;

    if (this.readmeForm.invalid) {
      this.readmeForm.markAllAsTouched();
      return;
    }

    const val = this.readmeForm.value;
    const readme: string = val.readme;

    if (this.shareData.shareInfo.readme == readme) { return; }
    if (this.ranForReadme == readme) { return; }

    this.ranForReadme == readme

    this.weShareClient.updateShare(this.shareData.shareInfo.id, null, null, readme)
      .subscribe(response => {
        this.shareData.shareInfo.readme = readme;
        this.errorCode = 200;
      }, (error: HttpErrorResponse) => {
        this.errorCode = error.status;
      });
  }

  get readme() {
    return this.readmeForm.get('readme')!;
  }
}
