import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { VideostorePage } from './videostore.page';
import {MediaService} from '../../services/media/media.service';
import {IonicModule} from '@ionic/angular';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {HttpClient} from '@angular/common/http';

const mockVideouploadService = jasmine.createSpyObj('VideouploadService', [ 'getAllVideos']);
mockVideouploadService.getAllVideos.and.callFake(
  (func)=>func([{name: 'testVideoName',dateStored: new Date(2021,6,21),id: 'testID'}])
);

describe('VideostorePage', () => {
  let component: VideostorePage;
  let fixture: ComponentFixture<VideostorePage>;

  const setBeforeEach=(imports, providers) =>{
    beforeEach(waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [ VideostorePage ],
        imports,
        providers
      }).compileComponents();

      fixture = TestBed.createComponent(VideostorePage);
      component = fixture.componentInstance;
      fixture.detectChanges();
    }));
  };

  describe('general',()=>{
    setBeforeEach([IonicModule.forRoot()], [
      {provide: MediaService, useValue: mockVideouploadService},
      {provide: HttpClient, useValue: HttpClientTestingModule}
    ]);

    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('get stored videos', () => {
      component.loadMoreData();
      expect(component.items[0][0].name).toBe('testVideoName');
      expect(component.items[0][0].dateStored.toDateString()).toBe((new Date(2021,6,21)).toDateString());
      expect(component.items[0][0].id).toBe('testID');
    });
  });

});
