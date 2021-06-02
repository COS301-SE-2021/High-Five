(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@angular/core'), require('nipplejs')) :
  typeof define === 'function' && define.amd ? define('ngx-joystick', ['exports', '@angular/core', 'nipplejs'], factory) :
  (global = typeof globalThis !== 'undefined' ? globalThis : global || self, factory(global['ngx-joystick'] = {}, global.ng.core, global.nipplejs));
}(this, (function (exports, core, nipplejs) { 'use strict';

  var NgxJoystickComponent = /** @class */ (function () {
      function NgxJoystickComponent(el) {
          this.el = el;
          this.move = new core.EventEmitter();
          // tslint:disable-next-line:no-output-native
          this.start = new core.EventEmitter();
          // tslint:disable-next-line:no-output-native
          this.end = new core.EventEmitter();
          this.dir = new core.EventEmitter();
          this.dirUp = new core.EventEmitter();
          this.dirDown = new core.EventEmitter();
          this.dirLeft = new core.EventEmitter();
          this.dirRight = new core.EventEmitter();
          this.plain = new core.EventEmitter();
          this.plainUp = new core.EventEmitter();
          this.plainDown = new core.EventEmitter();
          this.plainLeft = new core.EventEmitter();
          this.plainRight = new core.EventEmitter();
      }
      NgxJoystickComponent.prototype.ngOnInit = function () {
          if (!this.options) {
              this.options = this.getDefaultOptions();
          }
          else {
              this.options.zone = this.el.nativeElement;
          }
          this.manager = nipplejs.create(this.options);
          this.setupEvents();
      };
      NgxJoystickComponent.prototype.ngOnDestroy = function () {
          this.manager.destroy();
      };
      NgxJoystickComponent.prototype.getDefaultOptions = function () {
          var options = {
              zone: this.el.nativeElement,
              mode: 'static',
              position: { left: '50%', top: '50%' },
              color: 'blue'
          };
          return options;
      };
      NgxJoystickComponent.prototype.emitEvent = function (event, emitter) {
          var joystickEvent = { event: event.event, data: event.data };
          emitter.emit(joystickEvent);
      };
      NgxJoystickComponent.prototype.setupEvents = function () {
          var _this = this;
          this.manager.on('move', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.move); });
          this.manager.on('start', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.start); });
          this.manager.on('end', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.end); });
          this.manager.on('dir', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.dir); });
          this.manager.on('dir:up', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.dirUp); });
          this.manager.on('dir:down', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.dirDown); });
          this.manager.on('dir:left', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.dirLeft); });
          this.manager.on('dir:right', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.dirRight); });
          this.manager.on('plain', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.plain); });
          this.manager.on('plain:up', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.plainUp); });
          this.manager.on('plain:down', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.plainDown); });
          this.manager.on('plain:left', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.plainLeft); });
          this.manager.on('plain:right', function (event, data) { _this.emitEvent({ event: event, data: data }, _this.plainRight); });
      };
      return NgxJoystickComponent;
  }());
  NgxJoystickComponent.decorators = [
      { type: core.Component, args: [{
                  selector: 'ngx-joystick',
                  template: "\n  <div style=\"width: 100%; height: 100%\" id=\"static\"></div>\n  "
              },] }
  ];
  NgxJoystickComponent.ctorParameters = function () { return [
      { type: core.ElementRef }
  ]; };
  NgxJoystickComponent.propDecorators = {
      options: [{ type: core.Input }],
      move: [{ type: core.Output }],
      start: [{ type: core.Output }],
      end: [{ type: core.Output }],
      dir: [{ type: core.Output }],
      dirUp: [{ type: core.Output }],
      dirDown: [{ type: core.Output }],
      dirLeft: [{ type: core.Output }],
      dirRight: [{ type: core.Output }],
      plain: [{ type: core.Output }],
      plainUp: [{ type: core.Output }],
      plainDown: [{ type: core.Output }],
      plainLeft: [{ type: core.Output }],
      plainRight: [{ type: core.Output }]
  };

  var NgxJoystickModule = /** @class */ (function () {
      function NgxJoystickModule() {
      }
      return NgxJoystickModule;
  }());
  NgxJoystickModule.decorators = [
      { type: core.NgModule, args: [{
                  declarations: [NgxJoystickComponent],
                  imports: [],
                  exports: [NgxJoystickComponent]
              },] }
  ];

  /*
   * Public API Surface of ngx-joystick
   */

  /**
   * Generated bundle index. Do not edit.
   */

  exports.NgxJoystickComponent = NgxJoystickComponent;
  exports.NgxJoystickModule = NgxJoystickModule;

  Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=ngx-joystick.umd.js.map
