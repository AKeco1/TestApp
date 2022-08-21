import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-uploader-component',
  templateUrl: './uploader-component.component.html',
  styleUrls: ['./uploader-component.component.css']
})
export class UploaderComponentComponent implements OnInit {

  fileName = '';

  constructor(private http: HttpClient) { }

  uploadFile = (files: any) => {

    if (files.length === 0) {
      return;
    }

    console.log("File selected pressed button");

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    console.log("File selected pressed button CALL to API");

    console.log(formData);
    this.http.post('http://localhost:5400/api/upload', formData, { reportProgress: true, observe: 'events' })
        .subscribe({
          next: (event) => {
            //if (event.type === HttpEventType.UploadProgress)
             // this.progress = Math.round(100 * event.loaded / event.total);
            //else if (event.type === HttpEventType.Response) {
            //  this.message = 'Upload success.';
            //  this.onUploadFinished.emit(event.body);
            //}
          },
          error: (err: HttpErrorResponse) => console.log(err)
        });
  }

  ngOnInit() {

  }
}
