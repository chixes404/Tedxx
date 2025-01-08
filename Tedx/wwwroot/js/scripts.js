$(document).ready(function () {
    $('body').on('click', function () {
        $('.search_block').removeClass('visible');
    });

    $('.search_box').on('click', function (event) {
        event.stopPropagation();
    });

    $('.search_input').on('keyup', function (event) {
        if ($(this).val() !== '') {
            $(this).addClass('typing');
        } else {
            $(this).removeClass('typing');
        }
    });


    // timer countdown 
    const countDownClock = (number = 100, format = 'seconds') => {
        const d = document;
        const daysElement = d.querySelector('.days');
        const hoursElement = d.querySelector('.hours');
        const minutesElement = d.querySelector('.minutes');
        const secondsElement = d.querySelector('.seconds');
        let countdown;
        convertFormat(format);
        
        
        function convertFormat(format) {
          switch(format) {
            case 'seconds':
              return timer(number);
            case 'minutes':
              return timer(number * 60);
              case 'hours':
              return timer(number * 60 * 60);
            case 'days':
              return timer(number * 60 * 60 * 24);             
          }
        }
      
        function timer(seconds) {
          const now = Date.now();
          const then = now + seconds * 1000;
      
          countdown = setInterval(() => {
            const secondsLeft = Math.round((then - Date.now()) / 1000);
      
            if(secondsLeft <= 0) {
              clearInterval(countdown);
              return;
            };
      
            displayTimeLeft(secondsLeft);
      
          },1000);
        }
      
        function displayTimeLeft(seconds) {
          daysElement.textContent = Math.floor(seconds / 86400);
          hoursElement.textContent = Math.floor((seconds % 86400) / 3600);
          minutesElement.textContent = Math.floor((seconds % 86400) % 3600 / 60);
          secondsElement.textContent = seconds % 60 < 10 ? `0${seconds % 60}` : seconds % 60;
        }
      }
      
      
      /*
        start countdown
        enter number and format
        days, hours, minutes or seconds
      */
      countDownClock(20, 'days');


      // scroll btn 
      $(window).scroll(function () {
        if ($(this).scrollTop() > 600) {
          $('.scroll-top').fadeIn();
        } else {
          $('.scroll-top').fadeOut();
        }
      });
      $('.scroll-top').click(function () {
        $("html, body").animate({
          scrollTop: 0
        }, 100);
          return false;
      });

      // video
      // $('.myHTMLvideo').click(function() {
      //   if (this.paused) {
      //     $(this).attr('controls', true); // this will add controls attribute
      //    } else {
      //     $(this).removeAttr('controls'); // and this will remove controls attribute
      //    }
      // });

      // event speakers slider
       var mydir = $("html").attr("dir");
       if (mydir == 'rtl') {
        var rtlVal=true;
        }
        else{
            var rtlVal=false;
        }


      $(".interests-list li").click(function(){
        $(this).toggleClass('active');
      })  

      $(".event-speakers").owlCarousel({
        rtl:rtlVal,
        loop:true,
        autoplay:true,
        smartSpeed: 1500,
        responsiveClass:true,
        navText: ["<i class='fa-solid fa-caret-left'></i>","<i class='fa-solid fa-caret-right'></i>"],
        responsive:{
            0:{
                items:1,
                nav:true
            },
            500:{
                items:2,
                nav:true
            },
            800:{
              items:3,
              nav:true
            },
            1000:{
              items:4,
              nav:true
            },
            1300:{
                items:5,
                nav:true
            },
            1400:{
              items:6,
              nav:true
           }
        }
      })

      $(".newest-events").owlCarousel({
        rtl:rtlVal,
        loop:true,
        autoplay:true,
        smartSpeed: 1500,
        responsiveClass:true,
        margin:30,
        navText: ["<i class='fa-solid fa-caret-left'></i>","<i class='fa-solid fa-caret-right'></i>"],
        responsive:{
            0:{
                items:1,
                nav:true
            },
            600:{
                items:2,
                nav:true
            },
            1000:{
              items:3,
              nav:true
            }
        }
      })

      $(".speakers-slider.owl-carousel").owlCarousel({
        rtl:rtlVal,
        loop:true,
        autoplay:true,
        smartSpeed: 1500,
        responsiveClass:true,
        margin:30,
        dots:true,
        navText: ["<i class='fa-solid fa-caret-left'></i>","<i class='fa-solid fa-caret-right'></i>"],
        responsive:{
            0:{
                items:1,
                nav:true
            },
            600:{
                items:1,
                nav:true
            },
            1000:{
              items:2,
              nav:true,
            }
        }
      })
      
})