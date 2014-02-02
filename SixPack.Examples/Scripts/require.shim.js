var _build = typeof (window.viewBag) !== 'undefined' && typeof (window.viewBag.build) !== 'undefined' ? window.viewBag.build : '2050';

requirejs.config({
    //appDir: ".",
    baseUrl: "/scripts",
    urlArgs: 'v=' + _build,
    paths: {
        'linq': ['linq.min'],
        'transit': ['jquery.transit.min'],
        'scrollbar': ['jquery.mCustomScrollbar.concat.min'],
        'grain': ['grainjs/grain.min'],
        'grain.cache': ['grainjs/grain.min'],
        'grain.string': ['grainjs/grain.min'],
        'grain.uri': ['grainjs/grain.min'],
        'grain.wait': ['grainjs/grain.min'],
        'ko': ['knockout-2.2.1'],
        'ko.dragdrop': ['knockout.dragdrop'],
        'chatr.constants': ['constants']
    },
    //Remember: only use shim config for non-AMD scripts,
    //scripts that do not already call define(). The shim
    //config will not work correctly if used on AMD scripts,
    //in particular, the exports and init config will not
    //be triggered, and the deps config will be confusing
    //for those cases.
    shim: {
        'linq': {
            exports: 'Enumerable'
        }
    }
});

// globals (often referred to as exports) is a module to extend with anything that needs to support circular dependencies
// for instance, if two modules need to reference each other, each module can define a property 
// on globals as being equal to itself.  Then each module can reference the globals property
// which will have a value at some point. Exampes are below, under "globals examples"
define('globals', function () { return {}; });

define('jquery', function(){ return window.jQuery; });
define('signalr', ['jquery'], function() { return null; });
define('signalrHubs', ['jquery', 'signalr'], function () { return null; });

/////////////////////////////////////////////////////////
// globals examples:
//
//define('foo', ['globals'], function (globals) {
//    var $this = {};

//    $this.sayHello = function () {
//        globals.helloworld.hi('hello!');
//    };

//    $this.listen = function (words) {
//        console.log(words);
//    };

//    globals.foo = $this;
//    return $this;
//});

//define('helloworld', ['globals'], function (globals) {
//    var $this = {};

//    $this.hi = function (greeting) {
//        console.log(greeting);
//    };

//    $this.talkToFoo = function () {
//        globals.foo.listen('blah blah blah');
//    };

//    globals.helloworld = $this;
//    return $this;
//});